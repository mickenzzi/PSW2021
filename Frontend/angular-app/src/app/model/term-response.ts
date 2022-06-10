import { Doctor } from "./doctor";
import { User } from "./user";

export class TermResponse {
    Id: string = "";
    DateTimeTerm?: string;
    TermDoctor?: Doctor;
    TermUser?: User;
}
