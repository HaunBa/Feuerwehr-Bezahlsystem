import { Component, OnInit } from '@angular/core';
import { FormBuilder, FormControl, Validators } from '@angular/forms';
import { MatFormField } from '@angular/material/form-field';

@Component({
  selector: 'app-register',
  templateUrl: './register.component.html',
  styleUrls: ['./register.component.css']
})
export class RegisterComponent implements OnInit {
  registerForm = this.fb.group({
    email:[null, Validators.compose(Validators.required, Validators.email)],
    firstName: [null, Validators.required],
    lastName: [null, Validators.required],
    username: [null, Validators.required],
    password: [null, Validators.required],
    repPassword: [null, Validators.required]
  })

  constructor(private fb: FormBuilder) { }

  ngOnInit(): void {
  }
  getEmailErrorMessage(){
    let email = this.registerForm.controls['email'];

    if(email.errors?.['required']){
      return 'Bitte eine Emailadresse angeben';
    }
    if (email.errors?.['email']) {
      return 'Bitte eine gültige Emailadresse angeben';
    }

    return 'Error';
  }

  getUsernameErrorMessage(){
    let username = this.registerForm.controls['username'];

    if (username.errors?.['required']) {
      return 'Bitte einen Benutzernamen eingeben';
    }
    return '';
  }

  getFirstNameErrorMessage(){
    let firstName = this.registerForm.controls['firstName'];

    if (firstName.errors?.['required']) {
      return 'Bitte Vornamen eingeben';
    }
    return '';
  }

  getLastNameErrorMessage(){
    let lastName = this.registerForm.controls['lastName'];

    if (lastName.errors?.['required']) {
      return 'Bitte Nachnamen eingeben';
    }
    return '';
  }

  getPasswordErrorMessage(){
    let password = this.registerForm.controls['password'];

    if (password.errors?.['required']) {
      return 'Bitte Passwort eingeben';
    }
    return '';
  }
}
