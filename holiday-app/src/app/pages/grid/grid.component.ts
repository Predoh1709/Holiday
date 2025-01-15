import { Component, OnInit } from '@angular/core';
import { Holiday } from 'src/app/Models/Holiday';
import { HolidayService } from 'src/app/services/holiday.service';
import { MessageService } from 'primeng/api';

@Component({
  selector: 'app-grid',
  templateUrl: './grid.component.html',
  styleUrls: ['./grid.component.scss'],
  providers: [MessageService],
})
export class GridComponent implements OnInit{
  holidays: Holiday[] = [];
  selectedHoliday?: Holiday;
  description: string = '';
  isDialogVisible: boolean = false;

  constructor(private messageService: MessageService, private holidayService: HolidayService) {}

  ngOnInit() {
      this.loadHolidays();
  }

  loadHolidays(){
    this.holidayService.getHolidays().subscribe({
      next: (data) => (this.holidays = data),
      error: () => this.showError('Erro ao carregar feriados'), 
    });
  }

  editHoliday(holiday: Holiday) {
    this.selectedHoliday = holiday;
    this.description = holiday.description;
    this.isDialogVisible = true;
  }

  saveDescription() {
    if (this.selectedHoliday) {
      this.holidayService.updateHolidayDescription(this.selectedHoliday.id, this.description).subscribe({
        next: () => {
          this.showSuccess('Descrição atualizada com sucesso!');
          this.loadHolidays();
          this.selectedHoliday = undefined;
        },
        error: () => this.showError('Erro ao atualizar descrição.'),
      });
    }
    }
  
    // Remover feriado
    deleteHoliday(id: number) {
      this.holidayService.deleteHoliday(id).subscribe({
        next: () => {
          this.showSuccess('Feriado removido com sucesso!');
          this.loadHolidays();
        },
        error: (err) => {
            console.log('Erro ao remover feriado.', err);
            this.showError('Erro ao remover feriado.');
        }
      });
    }

    closeDialog() {
      this.isDialogVisible = false;
      this.selectedHoliday = undefined;
    }
  
    // Notificações
    showSuccess(detail: string) {
    this.messageService.add({ severity: 'success', summary: 'Sucesso', detail });
    }
  
    showError(detail: string) {
    this.messageService.add({ severity: 'error', summary: 'Erro', detail });
    }
  }
  