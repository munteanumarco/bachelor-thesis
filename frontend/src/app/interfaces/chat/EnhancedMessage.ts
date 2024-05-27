import { Message } from './message';

export interface EnhancedMessage extends Message {
  created_at: number;
}
