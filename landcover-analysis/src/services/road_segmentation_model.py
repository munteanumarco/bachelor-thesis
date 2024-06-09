import torch
import cv2
import os
import segmentation_models_pytorch as smp
import numpy as np
from utils.model_config import ModelConfig
from datetime import datetime

class RoadSegmentationModel:
    def __init__(self, model_path, model_config: ModelConfig, raw_data_folder_path, proccessed_data_folder_path,  device='cpu'):
        self.preprocessing_fn = smp.encoders.get_preprocessing_fn(model_config.encoder, model_config.encoder_weights)
        self.device = device
        self.model_config = model_config
        self.model = self.__load_model(model_path)
        self.class_names = ['road', 'background']
        self.class_rgb_values = [[255, 255, 255], [0, 0, 0]]
        self.select_classes = ['background', 'road']
        self.select_class_indices = [self.class_names.index(cls.lower()) for cls in self.select_classes]
        self.select_class_rgb_values =  np.array(self.class_rgb_values)[self.select_class_indices]
        self.raw_data_folder_path = raw_data_folder_path
        self.proccessed_data_folder_path = proccessed_data_folder_path

    def __load_model(self, model_path):
        chkpt = torch.load(model_path, map_location=self.device)
        model = smp.DeepLabV3Plus(
            encoder_name=self.model_config.encoder, 
            encoder_weights=self.model_config.encoder_weights, 
            classes=self.model_config.classes, 
            activation=self.model_config.activation,
        )
        model.load_state_dict(chkpt)
        model.eval()
        return model

    def __reverse_one_hot(self, image):
        x = np.argmax(image, axis = -1)
        return x

    def __colour_code_segmentation(self, image, label_values):
        colour_codes = np.array(label_values)
        x = colour_codes[image.astype(int)]
        return x

    def __to_tensor(self, x, **kwargs):
        return x.transpose(2, 0, 1).astype('float32')


    def predict(self, image_path):
        image_path = os.path.join(self.raw_data_folder_path, image_path)
        original_image = cv2.imread(image_path)
        img= cv2.resize(original_image,(1024,1024))
        img= cv2.cvtColor(img, cv2.COLOR_BGR2RGB)

        img= self.preprocessing_fn(img)
        x_tensor  = self.__to_tensor(img)
        x_tensor = torch.from_numpy(x_tensor).to(self.device).unsqueeze(0)
        pred_mask = self.model(x_tensor)
        pred_mask = pred_mask.detach().squeeze().cpu().numpy()
        pred_mask = np.transpose(pred_mask,(1,2,0))
        pred_mask = self.__colour_code_segmentation(self.__reverse_one_hot(pred_mask), self.select_class_rgb_values)

        idx = datetime.now().strftime("%Y%m%d%H%M%S")
        original_img_path = os.path.join(self.raw_data_folder_path, f"original_{idx}.png")
        cv2.imwrite(original_img_path, cv2.cvtColor(original_image, cv2.COLOR_RGB2BGR)) 
        pred_mask_path = os.path.join(self.proccessed_data_folder_path, f"predicted_{idx}.png")
        cv2.imwrite(pred_mask_path, pred_mask)
        return original_img_path, pred_mask_path