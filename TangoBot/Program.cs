using System;
using System.Net.Http;
using System.Threading.Tasks;
using HttpClientLib.TokenManagement;
using HttpClientLib.OrderApi;
using HttpClientLib.InstrumentApi;
using HttpClientLib.AccountApi;
using HttpClientLib.CustomerApi;
using TangoBotStreaming.Services;
using TangoBotAPI.Streaming;

using Microsoft.Extensions.DependencyInjection;
using System.Net.Http;
using TangoBot.HttpClientLib;
using TangoBotAPI.DI;
using TangoBotAPI.TokenManagement;
using HttpClientLib.OrderApi.Models;

namespace TangoBot
{
    class Program
    {
        static async Task Main(string[] args)
        {
            StartUp.InitializeDI();

            //MainManu();

            //TestStreaming().Wait();

            return;

            // Create HttpClient instance
            using var httpClient = new HttpClient();

            // Create a TokenProvider instance (assuming you have a suitable constructor)
            var tokenProvider = TangoBotServiceProvider.GetService<ITokenProvider>();

            // Create AccountComponent instance
            var accountComponent = new AccountComponent();

            // Test account number
            string accountNumber = "5WU34986";

            TestGetAccountBalanceAsync(accountComponent, accountNumber);
            /*
            // Fetch account information
            var accountInfo = await accountComponent.GetAccountBalancesAsync(accountNumber);

            // Print account information
            if (accountInfo != null)
            {
                Console.WriteLine("Account Information:");
                foreach (var kvp in accountInfo)
                {
                    Console.WriteLine($"{kvp.Key}: {kvp.Value}");
                }
            }
            else
            {
                Console.WriteLine("Failed to retrieve account information.");
            }
            */

            // Fetch balance snapshots
            await TestGetBalanceSnapshotAsync(accountComponent, accountNumber);

            await TestGetAccountPositionAsync(accountComponent, accountNumber);

            // Create CustomerComponent instance
            var customerComponent = new CustomerComponent();
            await TestGetCustomerInfoAsync(customerComponent);
            await TestGetCustomerAccountAsync(customerComponent);

            // Test customer ID and account number
            string customerId = "me"; // Assuming "me" is the customer ID of the currently authenticated customer
            string testAccountNumber = "5WU34986";
            await TestGetAccountAsync(customerComponent, customerId, testAccountNumber);

            // Create InstrumentComponent instance
            var instrumentComponent = new InstrumentComponent();

            // Test instrument symbol
            string instrumentSymbol = "AAPL";
            await TestGetInstrumentBySymbolAsync(instrumentComponent, instrumentSymbol);
            await TestGetActiveInstrumentsAsync(instrumentComponent);

            // Create OrderComponent instance
            var orderComponent = TangoBotServiceProvider.GetService<OrderComponent>();
            await TestGetAccountOrdersAsync(accountNumber, orderComponent);
            await TestGetAccountLiveOrdersAsync(accountNumber, orderComponent);
            await TestGetOrderByIdAsync(accountNumber, 155008, orderComponent);

            // Create an OrderRequest object
            var orderRequest = new OrderRequest
            {
                OrderType = "Limit",
                Price = 100.0,
                TimeInForce = "Day",
                PriceEffect = "Debit",
                Legs = new[]
                {
                    new LegRequest
                    {
                        Symbol = "AAPL",
                        InstrumentType = "Equity",
                        Action = "Buy to Open",
                        Quantity = 1
                    }
                }.ToList()
            };

            TestPostEquityDryRunOrderAsync(orderComponent, accountNumber, orderRequest).Wait();


            // Create an OrderRequest object
            var orderRequest2 = new OrderRequest
            {
                OrderType = "Limit",
                Price = 100.0,
                TimeInForce = "Day",
                PriceEffect = "Debit",
                Legs = new[]
                {
                    new LegRequest
                    {
                        Symbol = "X",
                        InstrumentType = "Equity",
                        Action = "Buy to Open",
                        Quantity = 1
                    }
                }.ToList()
            };

            TestPostEquityOrderAsync(orderComponent, accountNumber, orderRequest2).Wait();

            await TestCancelOrderByIdAsync(orderComponent, "5WU34986", 155672);

        }

        

        private static async void MainManu()
        {
            while (true)
            {
                Console.Clear();
                Console.WriteLine("Welcome to TangoBot!");
                Console.WriteLine("1. Create Instrument");
                Console.WriteLine("2. Catch Historic Data");
                Console.WriteLine("3. Tests");
                Console.WriteLine("4. Exit");
                Console.Write("Please select an option: ");

                var choice = Console.ReadLine();

                switch (choice)
                {
                    case "1":
                        await CreateInstrumentAsync();
                        break;
                    case "2":
                        await CatchHistoricDataAsync();
                        break;
                    case "3":
                        await OperateMenu();
                        break;
                    case "4":
                        return;
                    default:
                        Console.WriteLine("Invalid option. Please try again.");
                        break;
                }
            }
        }

        private static async Task OperateMenu()
        {
            while (true)
            {
                Console.Clear();
                Console.WriteLine("Tests Submenu");
                Console.WriteLine("1. Test Streaming");
                Console.WriteLine("2. Test Get Account Live Orders");
                Console.WriteLine("3. Test Get Account Orders");
                Console.WriteLine("4. Test Get Active Instruments");
                Console.WriteLine("5. Test Get Instrument By Symbol");
                Console.WriteLine("6. Test Get Account");
                Console.WriteLine("7. Test Get Customer Account");
                Console.WriteLine("8. Test Get Customer Info");
                Console.WriteLine("9. Test Get Account Position");
                Console.WriteLine("10. Test Get Balance Snapshot");
                Console.WriteLine("11. Test Get Account Balance");
                Console.WriteLine("12. Test Get Order By Id");
                Console.WriteLine("13. Test Post Equity Dry Run Order");
                Console.WriteLine("14. Test Post Equity Order");
                Console.WriteLine("15. Test Cancel Order By Id");
                Console.WriteLine("16. Back to Main Menu");
                Console.Write("Please select an option: ");

                var choice = Console.ReadLine();

                OrderComponent? orderComponent = TangoBotServiceProvider.GetService<OrderComponent>();
                switch (choice)
                {
                    case "1":
                        await TestStreaming();
                        break;
                    case "2":
                        await TestGetAccountLiveOrdersAsync("accountNumber", orderComponent);
                        break;
                    case "3":
                        await TestGetAccountOrdersAsync("accountNumber",
                                                        orderComponent: orderComponent);
                        break;
                    case "4":
                        await TestGetActiveInstrumentsAsync(new InstrumentComponent());
                        break;
                    case "5":
                        await TestGetInstrumentBySymbolAsync(new InstrumentComponent(), "instrumentSymbol");
                        break;
                    case "6":
                        await TestGetAccountAsync(new CustomerComponent(), "customerId", "testAccountNumber");
                        break;
                    case "7":
                        await TestGetCustomerAccountAsync(new CustomerComponent());
                        break;
                    case "8":
                        await TestGetCustomerInfoAsync(new CustomerComponent());
                        break;
                    case "9":
                        await TestGetAccountPositionAsync(new AccountComponent(), "accountNumber");
                        break;
                    case "10":
                        await TestGetBalanceSnapshotAsync(new AccountComponent(), "accountNumber");
                        break;
                    case "11":
                        TestGetAccountBalanceAsync(new AccountComponent(), "accountNumber");
                        break;
                    case "12":
                        await TestGetOrderByIdAsync("accountNumber", 123, orderComponent);
                        break;
                    case "13":
                        await TestPostEquityDryRunOrderAsync(orderComponent, "accountNumber", new OrderRequest());
                        break;
                    case "14":
                        await TestPostEquityOrderAsync(orderComponent, "accountNumber", new OrderRequest());
                        break;
                    case "15":
                        await TestCancelOrderByIdAsync(orderComponent, "accountNumber", 123);
                        break;
                    case "16":
                        return;
                    default:
                        Console.WriteLine("Invalid option. Please try again.");
                        break;
                }
            }
        }

        #region Main Menu

        private static async Task CreateInstrumentAsync()
        {
            Console.Write("Enter symbol: ");
            var symbol = Console.ReadLine();

            Console.Write("Enter from date (yyyy-MM-dd): ");
            var fromDateInput = Console.ReadLine();
            DateTime fromDate = DateTime.Parse(fromDateInput);

            Console.Write("Enter to date (yyyy-MM-dd): ");
            var toDateInput = Console.ReadLine();
            DateTime toDate = DateTime.Parse(toDateInput);

            Console.Write("Enter timeframe (D, W, M): ");
            var timeframeInput = Console.ReadLine();
            Timeframe timeframe = timeframeInput.ToUpper() switch
            {
                "D" => Timeframe.Daily,
                "W" => Timeframe.Weekly,
                "M" => Timeframe.Monthly,
                _ => throw new ArgumentOutOfRangeException(nameof(timeframeInput), $"Unsupported timeframe: {timeframeInput}")
            };

            // Assuming you have a method to create an instrument
            // await CreateInstrumentAsync(symbol, fromDate, toDate, timeframe);

            Console.WriteLine("Instrument created successfully.");
            Console.WriteLine("Press any key to return to the menu...");
            Console.ReadKey();
        }

        private static async Task CatchHistoricDataAsync()
        {
            Console.Write("Enter symbol: ");
            var symbol = Console.ReadLine();

            // Assuming you have a method to catch historic data
            // await CatchHistoricDataAsync(symbol);

            Console.WriteLine("Historic data caught successfully.");
            Console.WriteLine("Press any key to return to the menu...");
            Console.ReadKey();
        }

        #endregion

        private static async Task TestStreaming()
        {
            // Replace these values with valid ones
            string apiQuoteToken = "dGFzdHksYXBpLCwxNzMyMTA3MzcyLDE3MzIwMjA5NzIsVWNhMzJiYzg2LTIyOTgtNDlhYS1iYmY0LThjNDYxMTMwNjdlOQ.SyBAnxdcC3Xgpk99rUzH77barEzh81-0gkTqjXF0x8k";
            string webSocketUrl = "wss://tasty-openapi-ws.dxfeed.com/realtime";

            //IStreamService streamService = new StreamingService(webSocketUrl, apiQuoteToken);

            // Invoke StreamHistoricData method
            //var objs = streamService.StreamHistoricData("SPY", DateTime.Now.Date.AddDays(-5), DateTime.Now.Date, Timeframe.Daily, 1);

            //var eso = await streamService.StreamHistoricDataAsync("SPY", DateTime.Now.Date.AddYears(-10), DateTime.Now.Date, Timeframe.Daily, 1);

        }

        #region Tests

        private static async Task TestGetAccountLiveOrdersAsync(string accountNumber, OrderComponent orderComponent)
        {
            Console.WriteLine("\nFetching live orders...\n");
            // Fetch live orders for the account
            var liveOrders = await orderComponent.GetAccountLiveOrdersAsync(accountNumber);

            // Print live orders
            if (liveOrders != null && liveOrders.Length > 0)
            {
                Console.WriteLine("Live Orders:");
                foreach (var order in liveOrders)
                {
                    Console.WriteLine($"Order ID: {order.Id}");
                    Console.WriteLine($"Account Number: {order.AccountNumber}");
                    Console.WriteLine($"Cancellable: {order.Cancellable}");
                    Console.WriteLine($"Editable: {order.Editable}");
                    Console.WriteLine($"Edited: {order.Edited}");
                    Console.WriteLine($"External Client Order ID: {order.ExtClientOrderId}");
                    Console.WriteLine($"External Exchange Order Number: {order.ExtExchangeOrderNumber}");
                    Console.WriteLine($"External Global Order Number: {order.ExtGlobalOrderNumber}");
                    Console.WriteLine($"Global Request ID: {order.GlobalRequestId}");
                    Console.WriteLine($"Order Type: {order.OrderType}");
                    Console.WriteLine($"Price Effect: {order.PriceEffect}");
                    Console.WriteLine($"Received At: {order.ReceivedAt}");
                    Console.WriteLine($"Size: {order.Size}");
                    Console.WriteLine($"Status: {order.Status}");
                    Console.WriteLine($"Terminal At: {order.TerminalAt}");
                    Console.WriteLine($"Time In Force: {order.TimeInForce}");
                    Console.WriteLine($"Underlying Instrument Type: {order.UnderlyingInstrumentType}");
                    Console.WriteLine($"Underlying Symbol: {order.UnderlyingSymbol}");
                    Console.WriteLine($"Updated At: {order.UpdatedAt}");
                    Console.WriteLine("Legs:");
                    foreach (var leg in order.Legs)
                    {
                        Console.WriteLine($"  Action: {leg.Action}");
                        Console.WriteLine($"  Instrument Type: {leg.InstrumentType}");
                        Console.WriteLine($"  Quantity: {leg.Quantity}");
                        Console.WriteLine($"  Remaining Quantity: {leg.RemainingQuantity}");
                        Console.WriteLine($"  Symbol: {leg.Symbol}");
                        Console.WriteLine("  Fills:");
                        foreach (var fill in leg.Fills)
                        {
                            Console.WriteLine($"    Destination Venue: {fill.DestinationVenue}");
                            Console.WriteLine($"    External Exec ID: {fill.ExtExecId}");
                            Console.WriteLine($"    External Group Fill ID: {fill.ExtGroupFillId}");
                            Console.WriteLine($"    Fill ID: {fill.FillId}");
                            Console.WriteLine($"    Fill Price: {fill.FillPrice}");
                            Console.WriteLine($"    Filled At: {fill.FilledAt}");
                            Console.WriteLine($"    Quantity: {fill.Quantity}");
                        }
                    }
                }
            }
            else
            {
                Console.WriteLine("Failed to retrieve live orders or no live orders found.");
            }
        }

        private static async Task TestGetAccountOrdersAsync(string accountNumber, OrderComponent orderComponent)
        {
            Console.WriteLine("\nFetching account orders...\n");
            // Fetch orders for the account
            var orders = await orderComponent.GetAccountOrdersAsync(accountNumber);

            // Print orders
            if (orders != null && orders.Length > 0)
            {
                Console.WriteLine("Orders:");
                foreach (var order in orders)
                {
                    Console.WriteLine($"Order ID: {order.Id}");
                    Console.WriteLine($"Account Number: {order.AccountNumber}");
                    Console.WriteLine($"Cancellable: {order.Cancellable}");
                    Console.WriteLine($"Editable: {order.Editable}");
                    Console.WriteLine($"Edited: {order.Edited}");
                    Console.WriteLine($"External Client Order ID: {order.ExtClientOrderId}");
                    Console.WriteLine($"External Exchange Order Number: {order.ExtExchangeOrderNumber}");
                    Console.WriteLine($"External Global Order Number: {order.ExtGlobalOrderNumber}");
                    Console.WriteLine($"Global Request ID: {order.GlobalRequestId}");
                    Console.WriteLine($"Order Type: {order.OrderType}");
                    Console.WriteLine($"Price Effect: {order.PriceEffect}");
                    Console.WriteLine($"Received At: {order.ReceivedAt}");
                    Console.WriteLine($"Size: {order.Size}");
                    Console.WriteLine($"Status: {order.Status}");
                    Console.WriteLine($"Terminal At: {order.TerminalAt}");
                    Console.WriteLine($"Time In Force: {order.TimeInForce}");
                    Console.WriteLine($"Underlying Instrument Type: {order.UnderlyingInstrumentType}");
                    Console.WriteLine($"Underlying Symbol: {order.UnderlyingSymbol}");
                    Console.WriteLine($"Updated At: {order.UpdatedAt}");
                    Console.WriteLine("Legs:");
                    foreach (var leg in order.Legs)
                    {
                        Console.WriteLine($"  Action: {leg.Action}");
                        Console.WriteLine($"  Instrument Type: {leg.InstrumentType}");
                        Console.WriteLine($"  Quantity: {leg.Quantity}");
                        Console.WriteLine($"  Remaining Quantity: {leg.RemainingQuantity}");
                        Console.WriteLine($"  Symbol: {leg.Symbol}");
                        Console.WriteLine("  Fills:");
                        foreach (var fill in leg.Fills)
                        {
                            Console.WriteLine($"    Destination Venue: {fill.DestinationVenue}");
                            Console.WriteLine($"    External Exec ID: {fill.ExtExecId}");
                            Console.WriteLine($"    External Group Fill ID: {fill.ExtGroupFillId}");
                            Console.WriteLine($"    Fill ID: {fill.FillId}");
                            Console.WriteLine($"    Fill Price: {fill.FillPrice}");
                            Console.WriteLine($"    Filled At: {fill.FilledAt}");
                            Console.WriteLine($"    Quantity: {fill.Quantity}");
                        }
                    }
                }
            }
            else
            {
                Console.WriteLine("Failed to retrieve orders or no orders found.");
            }
        }

        private static async Task TestGetActiveInstrumentsAsync(InstrumentComponent instrumentComponent)
        {
            Console.WriteLine("\nFetching active instruments...\n");
            // Fetch active instruments
            var activeInstruments = await instrumentComponent.GetActiveInstrumentsAsync();

            // Print active instruments
            if (activeInstruments != null && activeInstruments.Count > 0)
            {
                Console.WriteLine("Active Instruments:");
                foreach (var activeInstrument in activeInstruments)
                {
                    Console.WriteLine($"Id: {activeInstrument.Id}");
                    Console.WriteLine($"Active: {activeInstrument.Active}");
                    Console.WriteLine($"Description: {activeInstrument.Description}");
                    Console.WriteLine($"Symbol: {activeInstrument.Symbol}");
                    // Add other properties as needed
                }
            }
            else
            {
                Console.WriteLine("Failed to retrieve active instruments or no active instruments found.");
            }
        }

        private static async Task TestGetInstrumentBySymbolAsync(InstrumentComponent instrumentComponent, string instrumentSymbol)
        {
            Console.WriteLine("\nFetching instrument information...\n");
            // Fetch instrument information
            var instrument = await instrumentComponent.GetInstrumentBySymbolAsync(instrumentSymbol);

            // Print instrument information
            if (instrument != null)
            {
                Console.WriteLine("Instrument Information:");
                Console.WriteLine($"Id: {instrument.Id}");
                Console.WriteLine($"Active: {instrument.Active}");
                Console.WriteLine($"Bypass Manual Review: {instrument.BypassManualReview}");
                Console.WriteLine($"Cusip: {instrument.Cusip}");
                Console.WriteLine($"Description: {instrument.Description}");
                Console.WriteLine($"Instrument Type: {instrument.InstrumentType}");
                Console.WriteLine($"Is Closing Only: {instrument.IsClosingOnly}");
                Console.WriteLine($"Is ETF: {instrument.IsEtf}");
                Console.WriteLine($"Is Fraud Risk: {instrument.IsFraudRisk}");
                Console.WriteLine($"Is Illiquid: {instrument.IsIlliquid}");
                Console.WriteLine($"Is Index: {instrument.IsIndex}");
                Console.WriteLine($"Is Options Closing Only: {instrument.IsOptionsClosingOnly}");
                Console.WriteLine($"Lendability: {instrument.Lendability}");
                Console.WriteLine($"Listed Market: {instrument.ListedMarket}");
                Console.WriteLine($"Market Time Instrument Collection: {instrument.MarketTimeInstrumentCollection}");
                Console.WriteLine($"Short Description: {instrument.ShortDescription}");
                Console.WriteLine($"Streamer Symbol: {instrument.StreamerSymbol}");
                Console.WriteLine($"Symbol: {instrument.Symbol}");

                Console.WriteLine("Option Tick Sizes:");
                foreach (var optionTickSize in instrument.OptionTickSizes)
                {
                    Console.WriteLine($"Threshold: {optionTickSize.Threshold}, Value: {optionTickSize.Value}");
                }

                Console.WriteLine("Tick Sizes:");
                foreach (var tickSize in instrument.TickSizes)
                {
                    Console.WriteLine($"Threshold: {tickSize.Threshold}, Value: {tickSize.Value}");
                }
            }
            else
            {
                Console.WriteLine("Failed to retrieve instrument information.");
            }
        }

        private static async Task TestGetAccountAsync(CustomerComponent customerComponent, string customerId, string testAccountNumber)
        {
            Console.WriteLine("\nFetching specific account information...\n");

            // Fetch specific account information
            var account = await customerComponent.GetAccountAsync(customerId, testAccountNumber);

            // Print specific account information
            if (account != null)
            {
                Console.WriteLine("Specific Account Information:");
                Console.WriteLine($"Account Number: {account.AccountNumber}");
                Console.WriteLine($"Account Type Name: {account.AccountTypeName}");
                Console.WriteLine($"Created At: {account.CreatedAt}");
                Console.WriteLine($"Day Trader Status: {account.DayTraderStatus}");
                Console.WriteLine($"Investment Objective: {account.InvestmentObjective}");
                Console.WriteLine($"Is Closed: {account.IsClosed}");
                Console.WriteLine($"Is Firm Error: {account.IsFirmError}");
                Console.WriteLine($"Is Firm Proprietary: {account.IsFirmProprietary}");
                Console.WriteLine($"Is Foreign: {account.IsForeign}");
                Console.WriteLine($"Is Futures Approved: {account.IsFuturesApproved}");
                Console.WriteLine($"Is Test Drive: {account.IsTestDrive}");
                Console.WriteLine($"Margin Or Cash: {account.MarginOrCash}");
                Console.WriteLine($"Nickname: {account.Nickname}");
                Console.WriteLine($"Opened At: {account.OpenedAt}");
                Console.WriteLine($"Suitable Options Level: {account.SuitableOptionsLevel}");
                Console.WriteLine($"Authority Level: {account.AuthorityLevel}");
            }
            else
            {
                Console.WriteLine("Failed to retrieve specific account information.");
            }
        }

        private static async Task TestGetCustomerAccountAsync(CustomerComponent customerComponent)
        {
            Console.WriteLine("\nFetching customer accounts information...\n");

            // Fetch customer accounts information
            var customerAccounts = await customerComponent.GetCustomerAccountsAsync();

            // Print customer accounts information
            if (customerAccounts != null)
            {
                Console.WriteLine("Customer Accounts:");
                foreach (var lAccount in customerAccounts)
                {
                    Console.WriteLine($"Account Id: {lAccount.AccountTypeName}");
                    Console.WriteLine($"Balance: {lAccount.AccountNumber}");
                    Console.WriteLine($"Currency: {lAccount.CreatedAt}");
                    // Add other properties as needed
                }
            }
            else
            {
                Console.WriteLine("Failed to retrieve customer accounts information.");
            }
        }

        private static async Task TestGetCustomerInfoAsync(CustomerComponent customerComponent)
        {
            Console.WriteLine("\nFetching customer information...\n");

            // Fetch customer information
            var customerInfo = await customerComponent.GetCustomerInfoAsync();

            // Print customer information
            if (customerInfo != null)
            {
                Console.WriteLine("Customer Information:");
                Console.WriteLine($"Id: {customerInfo.Id}");
                Console.WriteLine($"FirstName: {customerInfo.FirstName}");
                Console.WriteLine($"LastName: {customerInfo.LastName}");
                Console.WriteLine($"Email: {customerInfo.Email}");
                // Add other properties as needed
            }
            else
            {
                Console.WriteLine("Failed to retrieve customer information.");
            }
        }

        private static async Task TestGetAccountPositionAsync(AccountComponent accountComponent, string accountNumber)
        {
            Console.WriteLine("\nFetching account positions...\n");

            // Fetch account positions
            var accountPositions = await accountComponent.GetAccountPositionsAsync(accountNumber);

            // Print account positions
            if (accountPositions != null)
            {
                Console.WriteLine("Account Positions:");
                foreach (var position in accountPositions)
                {
                    Console.WriteLine("Position:");
                    foreach (var kvp in position)
                    {
                        Console.WriteLine($"{kvp.Key}: {kvp.Value}");
                    }
                }
            }
            else
            {
                Console.WriteLine("Failed to retrieve account positions.");
            }
        }

        private static async Task TestGetBalanceSnapshotAsync(AccountComponent accountComponent, string accountNumber)
        {
            // Fetch balance snapshots
            Console.WriteLine("\nFetching balance snapshots...\n");

            var balanceSnapshots = await accountComponent.GetBalanceSnapshotAsync(accountNumber);

            // Print balance snapshots
            if (balanceSnapshots != null)
            {
                Console.WriteLine("Balance Snapshots:");
                foreach (var snapshot in balanceSnapshots)
                {
                    Console.WriteLine("Snapshot:");
                    foreach (var kvp in snapshot)
                    {
                        Console.WriteLine($"{kvp.Key}: {kvp.Value}");
                    }
                }
            }
            else
            {
                Console.WriteLine("Failed to retrieve balance snapshots.");
            }
        }

        private static void TestGetAccountBalanceAsync(AccountComponent accountComponent, string accountNumber)
        {
            Console.WriteLine("\nFetching account balance...\n");
            // Fetch account information
            var accountInfo = accountComponent.GetAccountBalancesAsync(accountNumber).Result;

            // Print account information
            if (accountInfo != null)
            {
                Console.WriteLine("Account Information:");
                foreach (var kvp in accountInfo)
                {
                    Console.WriteLine($"{kvp.Key}: {kvp.Value}");
                }
            }
            else
            {
                Console.WriteLine("Failed to retrieve account information.");
            }
        }

        private static async Task TestGetOrderByIdAsync(string accountNumber, int orderId, OrderComponent orderComponent)
        {
            Console.WriteLine("\nFetching order by ID...\n");
            try
            {
                var order = await orderComponent.GetOrderByIdAsync(accountNumber, orderId);
                Console.WriteLine($"Order ID: {order.Id}");
                Console.WriteLine($"Account Number: {order.AccountNumber}");
                Console.WriteLine($"Status: {order.Status}");
                // Add more properties to display as needed
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error: {ex.Message}");
            }
        }

        private static async Task TestPostEquityDryRunOrderAsync(OrderComponent orderComponent, string accountNumber, OrderRequest orderRequest)
        {
            Console.WriteLine("\nTesting PostEquityDryRunOrder...\n");
            try
            {
                Console.WriteLine("[Info] Testing PostEquityDryRunOrder...");

                var dryRunReport = await orderComponent.PostEquityOrder(accountNumber, orderRequest);

                if (dryRunReport != null && dryRunReport.Data != null && dryRunReport.Data.Order != null)
                {
                    Console.WriteLine("[Success] Dry run order posted successfully.");
                    ReportData? data = dryRunReport.Data;
                    Console.WriteLine($"Order ID: {data.Order.Id}");
                    Console.WriteLine($"Warnings: {string.Join(", ", dryRunReport.Data.Warnings.Select(w => w.Message))}");
                }
                else
                {
                    Console.WriteLine("[Error] Dry run order failed.");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"[Error] Exception occurred while posting dry run order: {ex.Message}");
            }
        }

        private static async Task TestPostEquityOrderAsync(OrderComponent orderComponent, string accountNumber, OrderRequest orderRequest)
        {
            Console.WriteLine("\nTesting PostEquityOrder...\n");
            try
            {
                var orderPostReport = await orderComponent.PostEquityOrder(accountNumber, orderRequest, isDryRun: false) ?? throw new NullReferenceException();
                
                Console.WriteLine("[Success] Dry run order posted successfully.");
                Console.WriteLine($"Order ID: {orderPostReport.Data.Order.Id}");
                Console.WriteLine($"Warnings: {string.Join(", ", orderPostReport.Data.Warnings.Select(w => w.Message))}");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error placing order: {ex.Message}");
            }
        }

        private static async Task TestCancelOrderByIdAsync(OrderComponent orderComponent, string accountNumber, int orderId)
        {
            Console.WriteLine("\nTesting CancelOrderById...\n");
            try
            {
                var canceledOrder = await orderComponent.CancelOrderByIdAsync(accountNumber, orderId);
                Console.WriteLine("Order canceled successfully:");
                Console.WriteLine($"ID: {canceledOrder.Id}");
                Console.WriteLine($"Status: {canceledOrder.Status}");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error canceling order: {ex.Message}");
            }
        }
        
        #endregion
    }
}
