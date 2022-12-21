import { ComponentFixture, TestBed } from '@angular/core/testing';

import { PlayerStatisticsCardComponent } from './player-statistics-card.component';

describe('PlayerStatisticsCardComponent', () => {
  let component: PlayerStatisticsCardComponent;
  let fixture: ComponentFixture<PlayerStatisticsCardComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ PlayerStatisticsCardComponent ]
    })
    .compileComponents();

    fixture = TestBed.createComponent(PlayerStatisticsCardComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
