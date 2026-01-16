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
  import { useCreateAdvPreStatus, useUpdateAdvPreStatus } from "@/lib/abp/hooks/useAdvPreStatuses";
import { toast } from "sonner";

const formSchema = z.object({
  
    idStatus: z.any(),
  
    titulo: z.any(),
  
    ativo: z.any(),
  
    tsInclusao: z.any(),
  
    tsAlteracao: z.any(),
  
    ordem: z.any(),
  
    ultimo: z.any(),
  
    diasMaxParado: z.any(),
  
    idTipo: z.any(),
  
});

type FormValues = z.infer<typeof formSchema>;

interface AdvPreStatusFormProps {
  isOpen: boolean;
  onClose: () => void;
  initialValues ?: any;
}

export function AdvPreStatusForm({
  isOpen,
  onClose,
  initialValues,
}: AdvPreStatusFormProps) {
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

  const createMutation = useCreateAdvPreStatus();
const updateMutation = useUpdateAdvPreStatus();

const onSubmit = async (data: FormValues) => {
  try {
    if (isEditing) {
      await updateMutation.mutateAsync({ id: initialValues.id, data });
      toast.success("advPreStatus updated successfully");
    } else {
      await createMutation.mutateAsync(data);
      toast.success("advPreStatus created successfully");
    }
    onClose();
  } catch (error: any) {
    console.error("Failed to save advprestatus:", error);
    toast.error(error.message || "Failed to save advprestatus");
  }
};

return (
  <Dialog open= { isOpen } onOpenChange = { onClose } >
    <DialogContent className="sm:max-w-[500px]" >
      <DialogHeader>
      <DialogTitle>{ isEditing? "Edit advPreStatus": "Create advPreStatus" } </DialogTitle>
      <DialogDescription>
{ isEditing ? "Update the details of the advprestatus." : "Fill in the details to create a new advprestatus." }
</DialogDescription>
  </DialogHeader>
  < form onSubmit = { handleSubmit(onSubmit) } className = "space-y-4 py-4" >
    
<div className="space-y-2" >
  <Label htmlFor="idStatus" > idStatus</Label>

<Input id="idStatus" type = "number" step = "any" {...register("idStatus") } />

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
  <Label htmlFor="ultimo" > ultimo</Label>

<div className="flex items-center space-x-2 pt-1" >
  <Checkbox
                id="ultimo"
checked = { watch("ultimo") }
onCheckedChange = {(checked) => setValue("ultimo", !!checked)}
              />
  < label htmlFor = "ultimo" className = "text-sm font-normal" >
    { watch("ultimo") ?"Enabled": "Disabled" }
    </label>
    </div>

</div>

<div className="space-y-2" >
  <Label htmlFor="diasMaxParado" > diasMaxParado</Label>

<Input id="diasMaxParado" type = "number" step = "any" {...register("diasMaxParado") } />

</div>

<div className="space-y-2" >
  <Label htmlFor="idTipo" > idTipo</Label>

<Input id="idTipo" type = "number" step = "any" {...register("idTipo") } />

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
