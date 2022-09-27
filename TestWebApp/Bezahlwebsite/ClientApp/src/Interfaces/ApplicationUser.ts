import { IdentityUser } from "./IdentityUser";
import { Payment } from "./Payment";
import { TopUp } from "./TopUp";

export interface ApplicationUser extends IdentityUser {
  firstName: string;
  lastName: string;
  balance: number;
  comment: string;
  payments: Payment[];
  topUps: TopUp[];
  openCheckoutDate: string;
  openCheckoutValue: number;
}
