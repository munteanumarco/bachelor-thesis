import { EmergencyEventDto } from './EmergencyEventDto';

export interface EmergencyDetailsDto extends EmergencyEventDto {
  reportedByUsername: string;
  participantsCount: number;
  participantsUsernames: string[];
}
