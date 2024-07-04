import { LandCoverAnalysisStatus } from './LandcoverAnalysisStatus';

export interface LandCoverAnalysisDto {
  Id: string;
  EmergencyEventId: string;
  Status: LandCoverAnalysisStatus;
  TriggeredAt?: Date;
  CompletedAt?: Date;
  ProcessedImage?: string;
  RawImage?: string;
}
