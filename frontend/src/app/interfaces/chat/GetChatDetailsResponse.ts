import { BaseResponse } from '../BaseResponse';
import { ChatDetailsDto } from './ChatDetailsDto';

export interface GetChatDetailsResponse extends BaseResponse {
  data: ChatDetailsDto;
}
