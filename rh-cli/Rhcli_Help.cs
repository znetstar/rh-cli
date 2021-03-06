﻿// The MIT License (MIT)
// 
// Copyright (c) 2016 Brendan Chan
// 
// Permission is hereby granted, free of charge, to any person obtaining a copy
// of this software and associated documentation files (the "Software"), to deal
// in the Software without restriction, including without limitation the rights
// to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
// copies of the Software, and to permit persons to whom the Software is
// furnished to do so, subject to the following conditions:
// 
// The above copyright notice and this permission notice shall be included in
// all copies or substantial portions of the Software.
// 
// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
// IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
// FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
// AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
// LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
// OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN
// THE SOFTWARE.

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BasicallyMe.RobinhoodNet;

// Things I have yet to finish.
 
namespace rh_cli
{
    partial class Program
    {
        static void GetHelp()
        {
            Console.WriteLine(@"Usage: rh command
If 'command' is not any listed below it is assumed to be a quote

Commands:
<quote>         Bring up a real-time quote, where you can buy and sell shares.
                Append a ? after it to get more info on the stock in question.
                Enter more than one symbol to monitor multiple stocks in realtime.
orders <symbol> Bring up your recent orders for a particular symbol.
account         Brings up some of your basic account information.
positions       Bring up a list of your open positions, with  the  stock  you
                most recently bought for the first time appearing on top.
dividends       Returns dividends from each source, and total amount gained. 
                Ticker symbols marked with a star means you are within  their
                record date but not yet paid.
help            Brings up this text.
interactive     Bring up an interactive console combining real-time data; you 
                can enter app specific commands here.
query <query>   Gives you the top 10 results based on your search  query.  If 
                your query has spaces enclose your search term in quotes.
deposit         Allows you to send transfer money into RH. You will need  the 
                last four digits of the account number. You will be  able  to
                confirm before initating a transfer.
withdraw        Allows you to transfer money from RH into your bank.

Order Options:
gtc             Good Til Canceled
gfd             Good For Day - Expires at end of day.
stop            Stop Loss Order - Executes when price is belo w a  set  price
                when selling, and above a set price when buying. 5% collar.
stopl           Stop Limit Order. Same as Stop Loss  except  places  a  limit
                order at the specified price instead when the order executes.
ah              Allows your order to fill  during  extended  hours  (if  your
                account allows it). 

");
            
        }

        // Various warnings that I can think of, they may or may not be used in the app 

        void WarningHighVolatility()
        {
            Console.WriteLine("WARNING: This purchase will exceed your daytrading "+
                "buying power. If you are flagged as a pattern day trader before the end "+
                "of the trading day you will be given a day trading margin call for "+
                "the difference.\n\nDo you want to continue?");
        }

        void WarningBuyingUsingDTBP()
        {
            Console.WriteLine("WARNING: You are attempting to buy past your normal buying "+
                "power. As a pattern day trader, you must close these positions before the " +
                "end of the trading day. \n\nDo you want to continue?");
        }

        static void WarningCashViolation(bool filled = true)
        {
            Console.WriteLine("WARNING: You are buying this stock with "+
                "unsettled funds. {1}elling this position before funds are settled may "+
                "incur a 90-day restriction on your account.",
                filled ? "S" : " If this order executes, s"
                );
        }

        void WarningBuyLowMaintainence()
        {
            Console.WriteLine("WARNING: Placing this order will bring you very close "+
                "to maintainence margin requirements. This order may be cancelled if your "+
                "equity goes below maintainence requirements.");
        }

        void PromptRedistributeLimit()
        {
            Console.WriteLine("You have open limit sell orders that can prevent you " +
                "from making your trade. Do you want rh-cli to cancel your limit orders "+
                "for this stock?");
        }

    }
}