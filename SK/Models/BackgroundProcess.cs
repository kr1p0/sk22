namespace SK.Models
{
    public class BackgroundProcess : BackgroundService
    {



        public int IntervalSec { get; } = 60;

        protected async override Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                await Task.Delay(1000 * IntervalSec, stoppingToken);
                //Console.WriteLine("Check4Reminders");
                backgroundReminder();
            }
        }

        private async void backgroundReminder()
        {
            var ReminderList = Case.GetReminders();
            //Console.WriteLine("Reminders found: "+ ReminderList.Count);
            foreach (var reminder in ReminderList)
            {
                var parsed2Date = DateTime.Parse(reminder.EndDate);
                var timeLfet = parsed2Date.Subtract(CentralEuTimeZone.Get());

                if (timeLfet.TotalSeconds <= IntervalSec && timeLfet.TotalSeconds >= 0)
                {
                    wait4It((int)timeLfet.TotalMilliseconds, "wir-fred@tlen.pl", "SK", "Id: " + reminder.CaseId);
                }
            }

        }

        private async Task wait4It(int timeMs, string email, string title, string content)
        {

            await Task.Delay(timeMs);

            var mailList = new List<string>() { email };

            Email.sendMail(mailList, title, content);

            Console.WriteLine("Emails send");
        }
    }
}
