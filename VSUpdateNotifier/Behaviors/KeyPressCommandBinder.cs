using System.Collections.Generic;
using System.Windows.Input;

namespace VsUpdateNotifier.Behaviors
{
    public class KeyPressCommandBinder
    {
        public KeyPressCommandBinder()
        {
            this.bindingTable = new Dictionary<Key, IList<ICommand>>();
        }
        private IDictionary<Key, IList<ICommand>> bindingTable; 

        public IList<ICommand> ListCommand(Key key)
        {
            if (!this.bindingTable.ContainsKey(key))
            {
                this.bindingTable[key] = new List<ICommand>();
            }

            return this.bindingTable[key];
        }

        public void AddCommand(Key key, ICommand action)
        {
            if (!this.bindingTable.ContainsKey(key))
            {
                this.bindingTable[key] = new List<ICommand>();
            }

            var commands = this.bindingTable[key];
            commands.Add(action);
        }

        public void RemoveCommand(Key key, ICommand action)
        {
            if (!this.bindingTable.ContainsKey(key))
            {
                this.bindingTable[key] = new List<ICommand>();
            }

            var commands = this.bindingTable[key];
            commands.Remove(action);
        }

        public void RemoveAllCommand(Key key)
        {
            this.bindingTable[key] = new List<ICommand>();
        }
    }
}
