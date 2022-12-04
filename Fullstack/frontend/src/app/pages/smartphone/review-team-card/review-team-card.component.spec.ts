import { ComponentFixture, TestBed } from '@angular/core/testing';

import { ReviewTeamCardComponent } from './review-team-card.component';

describe('ReviewTeamCardComponent', () => {
  let component: ReviewTeamCardComponent;
  let fixture: ComponentFixture<ReviewTeamCardComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ ReviewTeamCardComponent ]
    })
    .compileComponents();

    fixture = TestBed.createComponent(ReviewTeamCardComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
