export interface Position {
  row: number;
  col: number;
}

export interface NQueensRequest {
  n: number;
  blockedPositions?: Position[];
  heuristicType: string;
}

export interface NQueensResponse {
  success: boolean;
  solution?: Position[];
  nodesExplored: number;
  executionTimeMs: number;
  heuristicUsed: string;
  message?: string;
}

export interface GuestPair {
  guest1: number;
  guest2: number;
}

export interface Guest {
  id: number;
  gender: string;
}

export interface SATRequest {
  totalGuests: number;
  totalTables: number;
  seatsPerTable: number;
  cannotSitTogether?: GuestPair[];
  mustSitTogether?: GuestPair[];
  guests?: Guest[];
  maxGenderDifference: number;
}

export interface SATResponse {
  success: boolean;
  tableAssignments?: { [key: number]: number[] };
  satFormula?: string;
  executionTimeMs: number;
  message?: string;
  totalClauses: number;
}

export interface CellPosition {
  row: number;
  col: number;
}

export interface Cage {
  sum: number;
  cells: CellPosition[];
}

export interface KillerSudokuRequest {
  grid: number[][];
  cages: Cage[];
}

export interface KillerSudokuResponse {
  success: boolean;
  solution?: number[][];
  backtrackCount: number;
  executionTimeMs: number;
  message?: string;
}

