from enum import Enum

class LandCoverAnalysisStatus(Enum):
  IN_PROGRESS = 1
  COMPLETED = 2
  FAILED = 3
  NOT_TRIGGERED = 4