import { IAtelier } from './IAtelier';

export interface IDag {
    date: string;
    // date: Date;
    // Datum ook als date opslaan in frontend, anders kan je op niks filteren, ffs
    ateliers: IAtelier[];
}
