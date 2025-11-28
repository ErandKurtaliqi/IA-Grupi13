import { Component, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { SATService } from '../../services/sat.service';
import { Guest, GuestPair } from '../../models/models';
import { NavbarComponent } from '../navbar/navbar.component';

@Component({
  selector: 'app-sat',
  standalone: true,
  imports: [CommonModule, FormsModule, NavbarComponent],
  templateUrl: './sat.component.html',
  styleUrls: ['./sat.component.css']
})
export class SATComponent implements OnInit {
  totalGuests: number = 12;
  totalTables: number = 3;
  seatsPerTable: number = 4;
  maxGenderDifference: number = 4;
  
  guests: Guest[] = [];
  cannotSitTogether: GuestPair[] = [];
  mustSitTogether: GuestPair[] = [];
  
  // For adding constraints
  guest1: number = 0;
  guest2: number = 1;
  
  // Results
  tableAssignments: { [key: number]: number[] } = {};
  satFormula: string = '';
  totalClauses: number = 0;
  executionTime: number = 0;
  message: string = '';
  isLoading: boolean = false;
  showSolution: boolean = false;
  showFormula: boolean = false;

  constructor(private satService: SATService) {}

  ngOnInit(): void {
    this.generateGuests();
    this.loadSampleData();
  }

  loadSampleData(): void {
    // Very simple constraints - guaranteed solvable
    this.cannotSitTogether = [
      { guest1: 0, guest2: 1 }
    ];
    
    this.mustSitTogether = [
      { guest1: 2, guest2: 3 }
    ];
  }

  generateGuests(): void {
    this.guests = [];
    for (let i = 0; i < this.totalGuests; i++) {
      this.guests.push({
        id: i,
        gender: Math.random() > 0.5 ? 'M' : 'F'
      });
    }
  }

  addCannotSitTogether(): void {
    if (this.guest1 !== this.guest2 && this.guest1 >= 0 && this.guest2 >= 0 
        && this.guest1 < this.totalGuests && this.guest2 < this.totalGuests) {
      const exists = this.cannotSitTogether.some(p => 
        (p.guest1 === this.guest1 && p.guest2 === this.guest2) ||
        (p.guest1 === this.guest2 && p.guest2 === this.guest1)
      );
      if (!exists) {
        this.cannotSitTogether.push({ guest1: this.guest1, guest2: this.guest2 });
      }
    }
  }

  addMustSitTogether(): void {
    if (this.guest1 !== this.guest2 && this.guest1 >= 0 && this.guest2 >= 0 
        && this.guest1 < this.totalGuests && this.guest2 < this.totalGuests) {
      const exists = this.mustSitTogether.some(p => 
        (p.guest1 === this.guest1 && p.guest2 === this.guest2) ||
        (p.guest1 === this.guest2 && p.guest2 === this.guest1)
      );
      if (!exists) {
        this.mustSitTogether.push({ guest1: this.guest1, guest2: this.guest2 });
      }
    }
  }

  removeCannotSit(index: number): void {
    this.cannotSitTogether.splice(index, 1);
  }

  removeMustSit(index: number): void {
    this.mustSitTogether.splice(index, 1);
  }

  clearConstraints(): void {
    this.cannotSitTogether = [];
    this.mustSitTogether = [];
  }

  generateRandomConstraints(): void {
    this.clearConstraints();
    const numConstraints = Math.floor(Math.random() * 5) + 3;
    
    for (let i = 0; i < numConstraints; i++) {
      const g1 = Math.floor(Math.random() * this.totalGuests);
      let g2 = Math.floor(Math.random() * this.totalGuests);
      while (g2 === g1) {
        g2 = Math.floor(Math.random() * this.totalGuests);
      }
      
      if (Math.random() > 0.5) {
        this.cannotSitTogether.push({ guest1: g1, guest2: g2 });
      } else {
        this.mustSitTogether.push({ guest1: g1, guest2: g2 });
      }
    }
  }

  solve(): void {
    this.isLoading = true;
    this.showSolution = false;
    this.message = '';

    const request = {
      totalGuests: this.totalGuests,
      totalTables: this.totalTables,
      seatsPerTable: this.seatsPerTable,
      maxGenderDifference: this.maxGenderDifference,
      guests: this.guests,
      cannotSitTogether: this.cannotSitTogether.length > 0 ? this.cannotSitTogether : undefined,
      mustSitTogether: this.mustSitTogether.length > 0 ? this.mustSitTogether : undefined
    };

    this.satService.solve(request).subscribe({
      next: (response) => {
        this.isLoading = false;
        this.showSolution = true;
        this.tableAssignments = response.tableAssignments || {};
        this.satFormula = response.satFormula || '';
        this.totalClauses = response.totalClauses;
        this.executionTime = response.executionTimeMs;
        this.message = response.message || '';
      },
      error: (error) => {
        this.isLoading = false;
        console.error('Full error:', error);
        if (error.error) {
          console.error('Error details:', error.error);
        }
        this.message = 'Error: ' + (error.error?.message || error.message || 'Failed to solve. Check console for details.');
      }
    });
  }

  getTableKeys(): number[] {
    return Object.keys(this.tableAssignments).map(k => parseInt(k)).sort((a, b) => a - b);
  }

  getGenderIcon(guestId: number): string {
    const guest = this.guests.find(g => g.id === guestId);
    return guest?.gender === 'M' ? '♂' : '♀';
  }

  getGenderColor(guestId: number): string {
    const guest = this.guests.find(g => g.id === guestId);
    return guest?.gender === 'M' ? '#3498db' : '#e91e63';
  }

  getTableGenderBalance(tableId: number): string {
    const guestIds = this.tableAssignments[tableId] || [];
    const males = guestIds.filter(id => this.guests.find(g => g.id === id)?.gender === 'M').length;
    const females = guestIds.length - males;
    return `${males}♂ / ${females}♀`;
  }
}

