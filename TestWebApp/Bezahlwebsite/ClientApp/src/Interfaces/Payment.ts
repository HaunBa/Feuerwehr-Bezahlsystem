import { ApplicationUser } from "./ApplicationUser";
import { BoughtArticle } from "./BoughtArticle";

export interface Payment {
  paymentId: number;
  personId: string;
  person: ApplicationUser;
  date: string;
  description: string;
  cashAmount: number;
  articles: BoughtArticle[];
  executorId: string | null;
  executor: ApplicationUser | null;
}
