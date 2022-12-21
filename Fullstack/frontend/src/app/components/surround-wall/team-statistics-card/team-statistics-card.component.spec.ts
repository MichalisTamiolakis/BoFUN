import { ComponentFixture, TestBed } from '@angular/core/testing';

import { TeamStatisticsCardComponent } from './team-statistics-card.component';

describe('TeamStatisticsCardComponent', () => {
  let component: TeamStatisticsCardComponent;
  let fixture: ComponentFixture<TeamStatisticsCardComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ TeamStatisticsCardComponent ]
    })
    .compileComponents();

    fixture = TestBed.createComponent(TeamStatisticsCardComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
