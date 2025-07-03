import { Component } from '@angular/core';
import { Routes } from '@angular/router';
import { LoginComponent } from './components/login/login.component';
import { HomepageComponent } from './components/homepage/homepage.component';
import { SignupComponent } from './components/signup/signup.component';
import { AtmComponent } from './components/atm/atm.component';
import { ProfileComponent } from './components/profile/profile.component';
import { WithdrawMoneyComponent } from './components/atm/withdraw-money/withdraw-money.component';
import { DepositMoneyComponent } from './components/atm/deposit-money/deposit-money.component';
import { MainpageComponent } from './components/profile/mainpage/mainpage.component';
import { CardsComponent } from './components/profile/cards/cards.component';
import { MoneyTransferComponent } from './components/profile/money-transfer/money-transfer.component';
import { ProfilepageComponent } from './components/profile/profilepage/profilepage.component';
import { AccounttransactionsComponent } from './components/profile/accounttransactions/accounttransactions.component';

export const routes: Routes = [
    {path:'', component: HomepageComponent},
    {path:'login', component: LoginComponent},
    {path:'signup', component: SignupComponent},
    {path:'atm', component: AtmComponent},
    {path:'homepage', component: HomepageComponent},
    {path:'profile', component: ProfileComponent},
    {path:'withdraw', component: WithdrawMoneyComponent},
    {path:'deposit', component: DepositMoneyComponent},
    {path:'mainpage', component: MainpageComponent},
    {path:'cards', component: CardsComponent},
    {path:'moneyTransfer', component: MoneyTransferComponent},
    {path:'profilepage', component: ProfilepageComponent},
    {path:'accounttransactions', component: AccounttransactionsComponent}
];
