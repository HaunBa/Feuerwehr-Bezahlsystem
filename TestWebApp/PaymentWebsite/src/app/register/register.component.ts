import { Component, OnInit } from '@angular/core';
import { FormControl, Validators } from '@angular/forms';
import { MatFormField } from '@angular/material/form-field';

@Component({
  selector: 'app-register',
  templateUrl: './register.component.html',
  styleUrls: ['./register.component.css']
})
export class RegisterComponent implements OnInit {
  email = new FormControl('', [Validators.required, Validators.email]);
  username = new FormControl('', [Validators.required]);
  firstName = new FormControl('', [Validators.required]);
  lastName = new FormControl('', [Validators.required]);
  password = new FormControl('', [Validators.required]);

  getEmailErrorMessage(){
    if (this.email.hasError('required')) {
      return 'Du musst eine Email Adresse angeben';
    }
    return this.email.hasError('email') ? 'Keine gültige Email' : '';
  }

  getUsernameErrorMessage(){
    return this.username.hasError('required') ? 'Du musst einen Benutzernamen eingeben.' : '';
  }

  getFirstNameErrorMessage(){
    return this.firstName.hasError('required') ? 'Du musst einen Vorname eingeben.' : '';
  }

  getLastNameErrorMessage(){
    return this.lastName.hasError('required') ? 'Du musst einen Nachnamen eingeben.' : '';
  }
  constructor() { }

  ngOnInit(): void {
  }

}
