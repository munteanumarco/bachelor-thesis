from dataclasses import dataclass

@dataclass(frozen=True)
class ModelConfig:
    encoder: str = 'resnet50'
    encoder_weights: str = 'imagenet'
    classes: int = 2
    activation: str = 'sigmoid'