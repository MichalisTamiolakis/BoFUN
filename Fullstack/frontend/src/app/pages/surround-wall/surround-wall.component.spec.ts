import { ComponentFixture, TestBed } from '@angular/core/testing';

import { SurroundWallComponent } from './surround-wall.component';

describe('SurroundWallComponent', () => {
  let component: SurroundWallComponent;
  let fixture: ComponentFixture<SurroundWallComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ SurroundWallComponent ]
    })
    .compileComponents();

    fixture = TestBed.createComponent(SurroundWallComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
