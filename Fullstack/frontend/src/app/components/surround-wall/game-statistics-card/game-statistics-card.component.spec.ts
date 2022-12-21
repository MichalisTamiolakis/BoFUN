import { ComponentFixture, TestBed } from '@angular/core/testing';

import { GameStatisticsCardComponent } from './game-statistics-card.component';

describe('GameStatisticsCardComponent', () => {
  let component: GameStatisticsCardComponent;
  let fixture: ComponentFixture<GameStatisticsCardComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ GameStatisticsCardComponent ]
    })
    .compileComponents();

    fixture = TestBed.createComponent(GameStatisticsCardComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
