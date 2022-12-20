import { ComponentFixture, TestBed } from '@angular/core/testing';

import { PantomimeMobileComponent } from './pantomime-mobile.component';

describe('PantomimeMobileComponent', () => {
  let component: PantomimeMobileComponent;
  let fixture: ComponentFixture<PantomimeMobileComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ PantomimeMobileComponent ]
    })
    .compileComponents();

    fixture = TestBed.createComponent(PantomimeMobileComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
