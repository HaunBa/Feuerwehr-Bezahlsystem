import { Payment } from "./Payment";
import { TopUp } from "./TopUp";

export interface UserWithAllInfosVM {
  id: string;
  username: string;
  firstName: string;
  lastName: string;
  balance: number;
  comment: string;
  openCheckoutDate: string;
  openCheckoutValue: number;
  role: string;
  payments: Payment[];
  topUps: TopUp[];
}
