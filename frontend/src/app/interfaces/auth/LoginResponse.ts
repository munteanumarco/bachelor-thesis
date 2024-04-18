import {BaseResponse} from "../BaseResponse";

export interface LoginResponse extends BaseResponse {
  token?: string;
}
