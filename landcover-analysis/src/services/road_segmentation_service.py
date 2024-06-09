import logging
from ai_model.road_segmentation_model import RoadSegmentationModel

class RoadSegmentationService:
    def __init__(self, road_segmentation_model: RoadSegmentationModel):
        self.road_segmentation_model = road_segmentation_model

    def segment_road(self, image_path):
        try:
            raw_image_path, proccessed_image_path = self.road_segmentation_model.predict(image_path)
            return raw_image_path, proccessed_image_path
        except Exception as e:
            logging.error(f"Error segmenting road: {e}")
            return None

    