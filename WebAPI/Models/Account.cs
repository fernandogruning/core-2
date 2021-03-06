using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;

namespace WebAPI.Models {
  public class Account {
    [Key]
    public int Id { get; set; }

    [Required]
    public DateTime CreatedAt { get; private set; }

    [Required]
    [Column(TypeName = "decimal(10,2)")]
    public decimal Balance { get; private set; }

    [Required]
    public int ClientId { get; set; }

    [ForeignKey("AccountId")]
    public virtual List<Loan> Loans { get; set; }

    [ForeignKey("AccountId")]
    public virtual List<Transaction> Transactions { get; set; }

    [ForeignKey("ReceiverAccountId")]
    public virtual List<LocalTransfer> ReceivedLocalTransfers { get; set; }

    public Account() {
      CreatedAt = DateTime.Now;
      Balance = 0;
    }

    public void UpdateBalance(decimal amount) {
      // refactor method to accept a Transaction &
      // do operation based on Transaction derived Type
      decimal newBalance = this.Balance + amount;
      if (newBalance < 0) {
        throw new Exception("Account does not have sufficient funds.");
      }
      this.Balance = newBalance;
    }

    public List<LocalTransfer> GetLocalTransfers() {
      return Transactions.OfType<LocalTransfer>().ToList();
    }
  }
}