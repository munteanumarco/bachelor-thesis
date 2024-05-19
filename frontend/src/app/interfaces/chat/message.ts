import { MessageType } from './messageType';

export interface Message {
  id: string;
  text: string;
  date: Date;
  username: string;
}
