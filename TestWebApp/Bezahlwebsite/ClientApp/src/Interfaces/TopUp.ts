import { ApplicationUser } from "./ApplicationUser";

export interface TopUp {
  topUpId: number;
  personId: string;
  person: ApplicationUser;
  date: string;
  description: string;
  cashAmount: number;
  executorId: string;
  executor: ApplicationUser;
}
