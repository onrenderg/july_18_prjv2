-- 23-Dec-2024 By Ravi 

[CLOSED]
1. Found that update Stock is allowing minus values.  (Fixed) which is allowng users to down the stock without any warning, which leads to be a bug alter allotment 
So we have fixed by applying client/server both side validation. Also Added refresh datatable as its not shoing updated record. 
[OEPN]
2. getBallotBoxListPendingForQR procedure (common 550 line) expecting int we are not sending and results exception
[CLOSED]
3. Add BallotBox Line 105 requres as commint . what will be reason (PARAMJEET)? -No-
[CLOSED]
4. PrintQR page condition when there is 0 in the result of dorpdown in line 306 then what to do?
[CLOSED]
5. ALTER PROCEDURE [sec].[UpdateStationeryStock] as it was not updating records. We needs id which needs to be deleted (which we have passed)