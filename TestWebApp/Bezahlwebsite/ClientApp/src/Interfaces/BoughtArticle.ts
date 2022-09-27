import { ArtType } from "./Enums";
import { Price } from "./Price";

export interface BoughtArticle {
  id: number;
  name: string;
  priceId: number;
  price: Price;
  amount: number;
  imageData: string;
  type: ArtType;
  active: boolean;
  isInVending: boolean;
  vendingSlot: number;
  vendingMachineNumber: number;
}
