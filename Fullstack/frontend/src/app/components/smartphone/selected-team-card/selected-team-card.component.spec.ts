import { ComponentFixture, TestBed } from '@angular/core/testing';

import { SelectedTeamCardComponent } from './selected-team-card.component';

describe('SelectedTeamCardComponent', () => {
  let component: SelectedTeamCardComponent;
  let fixture: ComponentFixture<SelectedTeamCardComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ SelectedTeamCardComponent ]
    })
    .compileComponents();

    fixture = TestBed.createComponent(SelectedTeamCardComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
