using Application.Shared.Exceptions;
using Domain.Items;

namespace Application.Items.Exceptions;

public class ItemNotFoundException(Guid id) : EntityNotFoundException<Item>(id);