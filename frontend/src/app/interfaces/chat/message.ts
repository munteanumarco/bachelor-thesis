import { MessageType } from './messageType';

export interface Message {
  id: string;
  text: string;
  date: Date;
  type: MessageType.TEXT;
  user: {
    name: string;
    avatar: string;
  };
}
