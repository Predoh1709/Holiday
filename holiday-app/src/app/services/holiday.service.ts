import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import { Holiday } from '../Models/Holiday';

@Injectable({
  providedIn: 'root',
})
export class HolidayService {
  private apiUrl = 'https://localhost:7087/api/Holiday'; // Base URL da API

  constructor(private http: HttpClient) {}

  getHolidays(): Observable<Holiday[]> {
	return this.http.get<Holiday[]>(this.apiUrl);
  }

  getHolidayById(id: number): Observable<Holiday> {
	return this.http.get<Holiday>(`${this.apiUrl}/${id}`);
  }

  updateHolidayDescription(id: number, description: string): Observable<void> {
	return this.http.put<void>(`${this.apiUrl}/${id}/description`, JSON.stringify(description), {
  	headers: { 'Content-Type': 'application/json-patch+json' },
	});
  }

  deleteHoliday(id: number): Observable<void> {
	return this.http.delete<void>(`${this.apiUrl}/${id}`);
  }
}