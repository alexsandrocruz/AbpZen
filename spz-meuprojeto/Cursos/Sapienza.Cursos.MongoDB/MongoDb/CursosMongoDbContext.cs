using MongoDB.Driver;
using Volo.Abp.Data;
using Volo.Abp.MongoDB;
using Volo.Chat.MongoDB;
using Volo.FileManagement.MongoDB;
using Sapienza.Cursos;
using Sapienza.Cursos.Turma;
using Sapienza.Cursos.Curso;
using Sapienza.Cursos.Aluno;

namespace Sapienza.Cursos.MongoDB
{
    [ConnectionStringName("Default")]
    public class CursosMongoDbContext : AbpMongoDbContext
    {
        // Add mongo collections here
        public IMongoCollection<Turma.Turma> Turmas => Collection<Turma.Turma>();
        public IMongoCollection<Curso.Curso> Cursos => Collection<Curso.Curso>();
        public IMongoCollection<Aluno.Aluno> Alunos => Collection<Aluno.Aluno>();

        protected override void CreateModel(IMongoModelBuilder modelBuilder)
        {
            base.CreateModel(modelBuilder);

            modelBuilder.ConfigureChat();
            modelBuilder.ConfigureFileManagement();

            modelBuilder.Entity<Turma.Turma>(b =>
            {
                b.CollectionName = CursosConsts.DbTablePrefix + "Turmas";
            });

            modelBuilder.Entity<Curso.Curso>(b =>
            {
                b.CollectionName = CursosConsts.DbTablePrefix + "Cursos";
            });

            modelBuilder.Entity<Aluno.Aluno>(b =>
            {
                b.CollectionName = CursosConsts.DbTablePrefix + "Alunos";
            });
        }
    }
}
