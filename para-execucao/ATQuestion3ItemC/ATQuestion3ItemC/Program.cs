using System.Collections.Generic;

// Definição básica de um literal
class Literal
{
    public string Name { get; }
    public bool Negated { get; }

    public Literal(string name, bool negated = false)
    {
        Name = name;
        Negated = negated;
    }

    public override string ToString()
    {
        return Negated ? $"¬{Name}" : Name;
    }
}

// Definição de uma conjunção (AND de literais)
class Conjunction
{
    public List<Literal> Literals { get; }

    public Conjunction(List<Literal> literals)
    {
        Literals = literals;
    }

    public override string ToString()
    {
        return $"({string.Join(" ∧ ", Literals)})";
    }

    // Avalia se a conjunção pode ser satisfeita e retorna uma valoração satisfatória
    public Dictionary<string, bool> AvaliarConjuncao()
    {
        Dictionary<string, bool> valoration = new Dictionary<string, bool>();

        foreach (var literal in Literals)
        {
            // Se o literal já foi avaliado antes, precisamos verificar a consistência da atribuição
            if (valoration.ContainsKey(literal.Name))
            {
                // Se houver uma contradição, a conjunção não pode ser satisfeita
                if (valoration[literal.Name] != !literal.Negated)
                {
                    return null; // Conjunção não pode ser satisfeita
                }
            }
            else
            {
                // Se o literal ainda não foi avaliado, atribuímos o valor necessário
                valoration[literal.Name] = !literal.Negated;
            }
        }

        // Se chegamos até aqui, a conjunção pode ser satisfeita
        return valoration;
    }
}

// Definição de uma disjunção de conjunções (FND)
class FND
{
    public List<Conjunction> Conjunctions { get; }

    public FND(List<Conjunction> conjunctions)
    {
        Conjunctions = conjunctions;
    }

    public override string ToString()
    {
        return $"{string.Join(" ∨ ", Conjunctions)}";
    }

    // Avalia a sentença em FND e retorna uma valoração que a satisfaça
    public Dictionary<string, bool> AvaliarSentenca()
    {
        foreach (var conjunction in Conjunctions)
        {
            var valoration = conjunction.AvaliarConjuncao();
            if (valoration != null)
            {
                return valoration; // Sentença é satisfeita por esta conjunção
            }
        }

        return null; // Nenhuma conjunção pôde ser satisfeita
    }
}

class Program
{
    static void Main(string[] args)
    {
        // Configura o console para usar UTF-8
        Console.OutputEncoding = System.Text.Encoding.UTF8;


        FND fnd = CreateFND();

        Console.WriteLine($"Sentença em FND: {fnd}");

        var resultado = fnd.AvaliarSentenca();

        if (resultado != null)
        {
            Console.WriteLine("A sentença pode ser satisfeita com a seguinte valoração:");
            foreach (var entry in resultado)
            {
                Console.WriteLine($"{entry.Key} = {entry.Value}");
            }
        }
        else
        {
            Console.WriteLine("Nenhuma valoração satisfaz a sentença.");
        }
    }

    private static FND CreateFND()
    {
        // Exemplo de sentença: (A ∧ ¬B) ∨ (¬A ∧ C)
        var fnd = new FND(new List<Conjunction>
        {
            new Conjunction(new List<Literal>
            {
                new Literal("A"),
                new Literal("B", true)
            }),
            new Conjunction(new List<Literal>
            {
                new Literal("A", true),
                new Literal("C")
            })
        });

        /*
            1. ¬A ∨ B
            2. ¬B ∨ C
            3. ¬C ∨ ¬A
        */
        var fnd1 = new FND(new List<Conjunction>{
            new Conjunction(new List<Literal>{
                new Literal("A", true),
                new Literal("B")
            })
        });

        var fnd2 = new FND(new List<Conjunction>{
            new Conjunction(new List<Literal>{
                new Literal("B", true),
                new Literal("C")
                }),
        });

        var fnd3 = new FND(new List<Conjunction>{
            new Conjunction(new List<Literal>{
                new Literal("C", true),
                new Literal("A", true)
                }),
        });

        return fnd3;
    }
}
