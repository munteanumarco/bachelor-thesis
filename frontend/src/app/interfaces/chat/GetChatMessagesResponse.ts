import { BaseResponse } from '../BaseResponse';
import { Message } from './message';

export interface GetChatMessagesResponse extends BaseResponse {
  chatMessages: Message[];
}
