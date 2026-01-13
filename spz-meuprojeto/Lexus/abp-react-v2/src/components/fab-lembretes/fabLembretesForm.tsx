import { useForm } from "react-hook-form";
  import { zodResolver } from "@hookform/resolvers/zod";
  import * as z from "zod";
  import {
    Dialog,
    DialogContent,
    DialogDescription,
    DialogFooter,
    DialogHeader,
    DialogTitle,
  } from "@/components/ui/dialog";
  import { Button } from "@/components/ui/button";
  import { Input } from "@/components/ui/input";
  import { Label } from "@/components/ui/label";
  import { Checkbox } from "@/components/ui/checkbox";
  import { Loader2 } from "lucide-react";
  import { useEffect } from "react";
  import { useCreateFabLembretes, useUpdateFabLembretes } from "@/lib/abp/hooks/useFabLembreteses";
import { toast } from "sonner";

const formSchema = z.object({
  
    idLembrete: z.any(),
  
    idUsuario: z.any(),
  
    mensagem: z.any(),
  
    destino: z.any(),
  
    tsInclusao: z.any(),
  
    incluidoPor: z.any(),
  
    lido: z.any(),
  
    tipo: z.any(),
  
});

type FormValues = z.infer<typeof formSchema>;

interface FabLembretesFormProps {
  isOpen: boolean;
  onClose: () => void;
  initialValues ?: any;
}

export function FabLembretesForm({
  isOpen,
  onClose,
  initialValues,
}: FabLembretesFormProps) {
  const isEditing = !!initialValues;

  const {
    register,
    handleSubmit,
    formState: { errors, isSubmitting },
    reset,
    setValue,
    watch,
  } = useForm<FormValues>({
    resolver: zodResolver(formSchema),
    defaultValues: initialValues || {},
  });

  useEffect(() => {
    if (initialValues) {
      reset(initialValues);
    } else {
      reset({});
    }
  }, [initialValues, reset]);

  const createMutation = useCreateFabLembretes();
const updateMutation = useUpdateFabLembretes();

const onSubmit = async (data: FormValues) => {
  try {
    if (isEditing) {
      await updateMutation.mutateAsync({ id: initialValues.id, data });
      toast.success("fabLembretes updated successfully");
    } else {
      await createMutation.mutateAsync(data);
      toast.success("fabLembretes created successfully");
    }
    onClose();
  } catch (error: any) {
    console.error("Failed to save fablembretes:", error);
    toast.error(error.message || "Failed to save fablembretes");
  }
};

return (
  <Dialog open= { isOpen } onOpenChange = { onClose } >
    <DialogContent className="sm:max-w-[500px]" >
      <DialogHeader>
      <DialogTitle>{ isEditing? "Edit fabLembretes": "Create fabLembretes" } </DialogTitle>
      <DialogDescription>
{ isEditing ? "Update the details of the fablembretes." : "Fill in the details to create a new fablembretes." }
</DialogDescription>
  </DialogHeader>
  < form onSubmit = { handleSubmit(onSubmit) } className = "space-y-4 py-4" >
    
<div className="space-y-2" >
  <Label htmlFor="idLembrete" > idLembrete</Label>

<Input id="idLembrete" type = "number" step = "any" {...register("idLembrete") } />

</div>

<div className="space-y-2" >
  <Label htmlFor="idUsuario" > idUsuario</Label>

<Input id="idUsuario" type = "number" step = "any" {...register("idUsuario") } />

</div>

<div className="space-y-2" >
  <Label htmlFor="mensagem" > mensagem</Label>

<Input id="mensagem" {...register("mensagem") } />

</div>

<div className="space-y-2" >
  <Label htmlFor="destino" > destino</Label>

<Input id="destino" {...register("destino") } />

</div>

<div className="space-y-2" >
  <Label htmlFor="tsInclusao" > tsInclusao</Label>

<Input id="tsInclusao" type = "date" {...register("tsInclusao") } />

</div>

<div className="space-y-2" >
  <Label htmlFor="incluidoPor" > incluidoPor</Label>

<Input id="incluidoPor" {...register("incluidoPor") } />

</div>

<div className="space-y-2" >
  <Label htmlFor="lido" > lido</Label>

<div className="flex items-center space-x-2 pt-1" >
  <Checkbox
                id="lido"
checked = { watch("lido") }
onCheckedChange = {(checked) => setValue("lido", !!checked)}
              />
  < label htmlFor = "lido" className = "text-sm font-normal" >
    { watch("lido") ?"Enabled": "Disabled" }
    </label>
    </div>

</div>

<div className="space-y-2" >
  <Label htmlFor="tipo" > tipo</Label>

<Input id="tipo" {...register("tipo") } />

</div>


<DialogFooter>
  <Button type="button" variant = "outline" onClick = { onClose } disabled = { isSubmitting } >
    Cancel
    </Button>
    < Button type = "submit" disabled = { isSubmitting } >
      { isSubmitting && <Loader2 className="mr-2 h-4 w-4 animate-spin" />}
{ isEditing ? "Save Changes" : "Create" }
</Button>
  </DialogFooter>
  </form>
  </DialogContent>
  </Dialog>
  );
}
