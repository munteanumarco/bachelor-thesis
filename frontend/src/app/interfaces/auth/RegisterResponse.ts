import { BaseResponse } from '../BaseResponse';
import { UserDto } from '../user/UserDto';

export interface RegisterResponse extends BaseResponse {
  user: UserDto;
}
