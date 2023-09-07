using Npgsql;

namespace Microsoft.eShopOnContainers.Services.Ordering.API.Application.Queries;

public class OrderQueries
    : IOrderQueries
{
    private string _connectionString = string.Empty;

    public OrderQueries(string constr)
    {
        _connectionString = !string.IsNullOrWhiteSpace(constr) ? constr : throw new ArgumentNullException(nameof(constr));
    }


    public async Task<Order> GetOrderAsync(int id)
    {
        using var connection = new NpgsqlConnection(_connectionString);
        connection.Open();

        var result = await connection.QueryAsync<dynamic>(
            @"select o.id as ordernumber, o.order_date as date, o.description as description,
                    o.address_city as city, o.address_country as country, o.address_state as state, o.address_street as street, o.address_zip_code as zipcode,
                    os.Name as status, 
                    oi.product_name as productname, oi.units as units, oi.unit_price as unitprice, oi.picture_url as pictureurl
                    FROM orders o
                    LEFT JOIN order_items oi ON o.Id = oi.order_id 
                    LEFT JOIN orderstatus os on o.order_status_id = os.id
                    WHERE o.id=@id"
                , new { id }
            );

        if (result.AsList().Count == 0)
            throw new KeyNotFoundException();

        return MapOrderItems(result);
    }

    public async Task<IEnumerable<OrderSummary>> GetOrdersFromUserAsync(Guid userId)
    {
        using var connection = new NpgsqlConnection(_connectionString);
        connection.Open();

        return await connection.QueryAsync<OrderSummary>(@"SELECT o.id as ordernumber,o.order_date as date,os.name as status, SUM(oi.units*oi.unit_price) as total
                    FROM orders o
                    LEFT JOIN order_items oi ON  o.id = oi.order_id 
                    LEFT JOIN orderstatus os on o.order_status_id = os.id                     
                    LEFT JOIN buyers ob on o.buyer_id = ob.id
                    WHERE ob.identity_guid::uuid = @userId
                    GROUP BY o.id, o.order_date, os.name 
                    ORDER BY o.id", new { userId });
    }

    public async Task<IEnumerable<CardType>> GetCardTypesAsync()
    {
        using var connection = new NpgsqlConnection(_connectionString);
        connection.Open();

        return await connection.QueryAsync<CardType>("SELECT * FROM cardtypes");
    }

    private Order MapOrderItems(dynamic result)
    {
        var order = new Order
        {
            ordernumber = result[0].ordernumber,
            date = result[0].date,
            status = result[0].status,
            description = result[0].description,
            street = result[0].street,
            city = result[0].city,
            zipcode = result[0].zipcode,
            country = result[0].country,
            orderitems = new List<Orderitem>(),
            total = 0
        };

        foreach (dynamic item in result)
        {
            var orderitem = new Orderitem
            {
                productname = item.productname,
                units = item.units,
                unitprice = (double)item.unitprice,
                pictureurl = item.pictureurl
            };

            order.total += item.units * item.unitprice;
            order.orderitems.Add(orderitem);
        }

        return order;
    }
}
