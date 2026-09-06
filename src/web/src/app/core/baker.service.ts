import { Injectable } from '@angular/core';
import { BakerSettings } from './baker';

const SETTINGS: BakerSettings = {
  monthlyIncome: 4000,
  hoursPerDay: 6,
  daysPerWeek: 5,
  monthlyFixedCosts: 1250,
  defaultMarkup: 100,
  cardFee: 3.5,
  tax: 6,
};

@Injectable({ providedIn: 'root' })
export class BakerService {
  async load(): Promise<BakerSettings> {
    await delay(250);

    return SETTINGS;
  }
}

function delay(milliseconds: number): Promise<void> {
  return new Promise(resolve => setTimeout(resolve, milliseconds));
}
