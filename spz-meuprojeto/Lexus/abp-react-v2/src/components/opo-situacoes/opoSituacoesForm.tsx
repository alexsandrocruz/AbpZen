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
  import { useCreateOpoSituacoes, useUpdateOpoSituacoes } from "@/lib/abp/hooks/useOpoSituacoeses";
import { toast } from "sonner";

const formSchema = z.object({
  
    idSituacao: z.any(),
  
    titulo: z.any(),
  
    ativo: z.any(),
  
    tsInclusao: z.any(),
  
    tsAlteracao: z.any(),
  
    ordem: z.any(),
  
    considerarIndicador: z.any(),
  
});

type FormValues = z.infer<typeof formSchema>;

interface OpoSituacoesFormProps {
  isOpen: boolean;
  onClose: () => void;
  initialValues ?: any;
}

export function OpoSituacoesForm({
  isOpen,
  onClose,
  initialValues,
}: OpoSituacoesFormProps) {
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

  const createMutation = useCreateOpoSituacoes();
const updateMutation = useUpdateOpoSituacoes();

const onSubmit = async (data: FormValues) => {
  try {
    if (isEditing) {
      await updateMutation.mutateAsync({ id: initialValues.id, data });
      toast.success("opoSituacoes updated successfully");
    } else {
      await createMutation.mutateAsync(data);
      toast.success("opoSituacoes created successfully");
    }
    onClose();
  } catch (error: any) {
    console.error("Failed to save oposituacoes:", error);
    toast.error(error.message || "Failed to save oposituacoes");
  }
};

return (
  <Dialog open= { isOpen } onOpenChange = { onClose } >
    <DialogContent className="sm:max-w-[500px]" >
      <DialogHeader>
      <DialogTitle>{ isEditing? "Edit opoSituacoes": "Create opoSituacoes" } </DialogTitle>
      <DialogDescription>
{ isEditing ? "Update the details of the oposituacoes." : "Fill in the details to create a new oposituacoes." }
</DialogDescription>
  </DialogHeader>
  < form onSubmit = { handleSubmit(onSubmit) } className = "space-y-4 py-4" >
    
<div className="space-y-2" >
  <Label htmlFor="idSituacao" > idSituacao</Label>

<Input id="idSituacao" type = "number" step = "any" {...register("idSituacao") } />

</div>

<div className="space-y-2" >
  <Label htmlFor="titulo" > titulo</Label>

<Input id="titulo" {...register("titulo") } />

</div>

<div className="space-y-2" >
  <Label htmlFor="ativo" > ativo</Label>

<div className="flex items-center space-x-2 pt-1" >
  <Checkbox
                id="ativo"
checked = { watch("ativo") }
onCheckedChange = {(checked) => setValue("ativo", !!checked)}
              />
  < label htmlFor = "ativo" className = "text-sm font-normal" >
    { watch("ativo") ?"Enabled": "Disabled" }
    </label>
    </div>

</div>

<div className="space-y-2" >
  <Label htmlFor="tsInclusao" > tsInclusao</Label>

<Input id="tsInclusao" type = "date" {...register("tsInclusao") } />

</div>

<div className="space-y-2" >
  <Label htmlFor="tsAlteracao" > tsAlteracao</Label>

<Input id="tsAlteracao" type = "date" {...register("tsAlteracao") } />

</div>

<div className="space-y-2" >
  <Label htmlFor="ordem" > ordem</Label>

<Input id="ordem" type = "number" step = "any" {...register("ordem") } />

</div>

<div className="space-y-2" >
  <Label htmlFor="considerarIndicador" > considerarIndicador</Label>

<div className="flex items-center space-x-2 pt-1" >
  <Checkbox
                id="considerarIndicador"
checked = { watch("considerarIndicador") }
onCheckedChange = {(checked) => setValue("considerarIndicador", !!checked)}
              />
  < label htmlFor = "considerarIndicador" className = "text-sm font-normal" >
    { watch("considerarIndicador") ?"Enabled": "Disabled" }
    </label>
    </div>

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
