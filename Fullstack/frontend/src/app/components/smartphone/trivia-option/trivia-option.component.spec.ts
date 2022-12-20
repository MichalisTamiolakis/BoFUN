import { ComponentFixture, TestBed } from '@angular/core/testing';

import { TriviaOptionComponent } from './trivia-option.component';

describe('TriviaOptionComponent', () => {
  let component: TriviaOptionComponent;
  let fixture: ComponentFixture<TriviaOptionComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ TriviaOptionComponent ]
    })
    .compileComponents();

    fixture = TestBed.createComponent(TriviaOptionComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
