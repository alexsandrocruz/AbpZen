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
  import { useCreateAdvPreLogStatus, useUpdateAdvPreLogStatus } from "@/lib/abp/hooks/useAdvPreLogStatuses";
import { toast } from "sonner";

const formSchema = z.object({
  
    idLog: z.any(),
  
    idProcesso: z.any(),
  
    idStatus: z.any(),
  
    tsInclusao: z.any(),
  
    conversao: z.any(),
  
    tsConversao: z.any(),
  
    perdido: z.any(),
  
    tsPerdido: z.any(),
  
    diasCorridosDoAnterior: z.any(),
  
    usuario: z.any(),
  
});

type FormValues = z.infer<typeof formSchema>;

interface AdvPreLogStatusFormProps {
  isOpen: boolean;
  onClose: () => void;
  initialValues ?: any;
}

export function AdvPreLogStatusForm({
  isOpen,
  onClose,
  initialValues,
}: AdvPreLogStatusFormProps) {
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

  const createMutation = useCreateAdvPreLogStatus();
const updateMutation = useUpdateAdvPreLogStatus();

const onSubmit = async (data: FormValues) => {
  try {
    if (isEditing) {
      await updateMutation.mutateAsync({ id: initialValues.id, data });
      toast.success("advPreLogStatus updated successfully");
    } else {
      await createMutation.mutateAsync(data);
      toast.success("advPreLogStatus created successfully");
    }
    onClose();
  } catch (error: any) {
    console.error("Failed to save advprelogstatus:", error);
    toast.error(error.message || "Failed to save advprelogstatus");
  }
};

return (
  <Dialog open= { isOpen } onOpenChange = { onClose } >
    <DialogContent className="sm:max-w-[500px]" >
      <DialogHeader>
      <DialogTitle>{ isEditing? "Edit advPreLogStatus": "Create advPreLogStatus" } </DialogTitle>
      <DialogDescription>
{ isEditing ? "Update the details of the advprelogstatus." : "Fill in the details to create a new advprelogstatus." }
</DialogDescription>
  </DialogHeader>
  < form onSubmit = { handleSubmit(onSubmit) } className = "space-y-4 py-4" >
    
<div className="space-y-2" >
  <Label htmlFor="idLog" > idLog</Label>

<Input id="idLog" type = "number" step = "any" {...register("idLog") } />

</div>

<div className="space-y-2" >
  <Label htmlFor="idProcesso" > idProcesso * </Label>

<Input id="idProcesso" type = "number" step = "any" {...register("idProcesso") } />

</div>

<div className="space-y-2" >
  <Label htmlFor="idStatus" > idStatus * </Label>

<Input id="idStatus" type = "number" step = "any" {...register("idStatus") } />

</div>

<div className="space-y-2" >
  <Label htmlFor="tsInclusao" > tsInclusao</Label>

<Input id="tsInclusao" type = "date" {...register("tsInclusao") } />

</div>

<div className="space-y-2" >
  <Label htmlFor="conversao" > conversao</Label>

<div className="flex items-center space-x-2 pt-1" >
  <Checkbox
                id="conversao"
checked = { watch("conversao") }
onCheckedChange = {(checked) => setValue("conversao", !!checked)}
              />
  < label htmlFor = "conversao" className = "text-sm font-normal" >
    { watch("conversao") ?"Enabled": "Disabled" }
    </label>
    </div>

</div>

<div className="space-y-2" >
  <Label htmlFor="tsConversao" > tsConversao</Label>

<Input id="tsConversao" type = "date" {...register("tsConversao") } />

</div>

<div className="space-y-2" >
  <Label htmlFor="perdido" > perdido</Label>

<div className="flex items-center space-x-2 pt-1" >
  <Checkbox
                id="perdido"
checked = { watch("perdido") }
onCheckedChange = {(checked) => setValue("perdido", !!checked)}
              />
  < label htmlFor = "perdido" className = "text-sm font-normal" >
    { watch("perdido") ?"Enabled": "Disabled" }
    </label>
    </div>

</div>

<div className="space-y-2" >
  <Label htmlFor="tsPerdido" > tsPerdido</Label>

<Input id="tsPerdido" type = "date" {...register("tsPerdido") } />

</div>

<div className="space-y-2" >
  <Label htmlFor="diasCorridosDoAnterior" > diasCorridosDoAnterior</Label>

<Input id="diasCorridosDoAnterior" type = "number" step = "any" {...register("diasCorridosDoAnterior") } />

</div>

<div className="space-y-2" >
  <Label htmlFor="usuario" > usuario</Label>

<Input id="usuario" {...register("usuario") } />

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
