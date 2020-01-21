using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BR.DTO;
using BR.EF;
using BR.Models;
using BR.Services.Interfaces;
using Newtonsoft.Json;

namespace BR.Services
{
    public class SchemeService : ISchemaService
    {
        private readonly IAsyncRepository _repository;

        public SchemeService(IAsyncRepository repository)
        {
            _repository = repository;
        }
        public async Task AddNewSchema(NewSchemaRequest newScheme, string clientIdentityId)
        {
            //var client = await _repository.GetClient(clientIdentityId);
            //if(client is null)
            //{
            //    return;
            //}

            //var floor = await _repository.GetFloor(client.Id, newScheme.FloorNumber);
            //if(floor is null)
            //{
            //    floor = await _repository.AddFloor(new Floor()
            //    {
            //        ClientId = client.Id,
            //        Number = newScheme.FloorNumber
            //    });
            //}

            var floor = await _repository.GetFloor(17, newScheme.FloorNumber);
            if (floor is null)
            {
                floor = await _repository.AddFloor(new Floor()
                {
                    ClientId = 17,
                    Number = newScheme.FloorNumber
                });
            }

            var hall = await _repository.AddHall(new Hall()
            {
                FloorId = floor.Id,
                Title = newScheme.HallTitle,
                JsonInfo = "new"
            });
            
            var tableState = await _repository.GetTableState("idle");
            foreach(var node in newScheme.TableArray)
            {
                var table = new Table()
                {
                    HallId = hall.Id,
                    TableStateId = tableState.Id,
                    MaxGuests = node.MaxGuests,
                    MinGuests = node.MinGuests,
                    Number = node.Number
                };
                var newTable = await _repository.AddTable(table);
                node.Id = newTable.Id;
            }

            hall.JsonInfo = JsonConvert.SerializeObject(newScheme);
            var res = await _repository.UpdateHall(hall);
        }

        public async Task UpdateSchema(UpdateSchemaRequest updateSchemeRequest)
        {
            var hall = await _repository.GetHall(updateSchemeRequest.HallId);
            if(hall is null)
            {
                return;
            }
            var tableState = await _repository.GetTableState("idle");
            if(tableState is null)
            {
                return;
            }
            foreach(var node in updateSchemeRequest.TableArray)
            {
                if(node.Id is null)
                {
                    var table = await _repository.AddTable(
                        new Table()
                        {
                            HallId = hall.Id,
                            MaxGuests = node.MaxGuests,
                            MinGuests = node.MinGuests,
                            Number = node.Number,
                            TableStateId = tableState.Id
                        });
                    node.Id = table.Id;
                }
                else
                {
                    var table = await _repository.GetTable(node.Id ?? default(int));
                    if(table is null)
                    {
                        continue;
                    }
                    table.MaxGuests = node.MaxGuests;
                    table.MinGuests = node.MinGuests;
                    table.Number = node.Number;
                    await _repository.UpdateTable(table);
                }
            }

            var hallTables = hall.Tables;
            var tablesToDelete = new List<Table>();
            foreach(var table in hallTables)
            {
                if (updateSchemeRequest.TableArray.Any(item => table.Id == item.Id))
                {
                    continue;
                }
                tablesToDelete.Add(table);
            }
            foreach(var table in tablesToDelete)
            {
                await _repository.DeleteTable(table);
            }
            hall.JsonInfo = JsonConvert.SerializeObject(updateSchemeRequest);
            hall.Title = updateSchemeRequest.HallTitle;
            var res = await _repository.UpdateHall(hall);
        }
    }
}
