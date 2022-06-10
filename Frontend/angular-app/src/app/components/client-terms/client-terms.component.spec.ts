import { ComponentFixture, TestBed } from '@angular/core/testing';

import { ClientTermsComponent } from './client-terms.component';

describe('ClientTermsComponent', () => {
  let component: ClientTermsComponent;
  let fixture: ComponentFixture<ClientTermsComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ ClientTermsComponent ]
    })
    .compileComponents();
  });

  beforeEach(() => {
    fixture = TestBed.createComponent(ClientTermsComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
