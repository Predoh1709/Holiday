import { VariableDate } from "./VariableDate";

export interface Holiday {
  id: number;
  date: string;
  title: string;
  description: string;
  legislation: string;
  type: string;
  variableDates: VariableDate[];
}