import { IPersoon } from './IPersoon';

export interface IDagAtelierTemp {
    clienten: IPersoon[];
    begeleiders: IPersoon[];
    atelierID: number;
    naam: string;
    start?: string;
    end?: string;
}
