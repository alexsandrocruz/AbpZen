import { useQuery, useMutation, useQueryClient } from "@tanstack/react-query";
import { apiClient } from "../api-client";

interface GetProposalTemplateBlocksInput {
  filter ?: string;
  skipCount ?: number;
  maxResultCount ?: number;
  }

export function useProposalTemplateBlocks(input: GetProposalTemplateBlocksInput = {}) {
  const { filter, skipCount = 0, maxResultCount = 10, ...rest } = input;

  return useQuery({
    queryKey: ["proposalTemplateBlocks", filter, skipCount, maxResultCount, rest],
    queryFn: async () => {
      const response = await apiClient.get("/api/app/proposal-template-block", {
        params: {
          filter,
          skipCount,
          maxResultCount,
          ...rest,
        },
      });
      return response.data;
    },
  });
}

export function useAllProposalTemplateBlocks() {
  return useQuery({
    queryKey: ["proposalTemplateBlocks", "all"],
    queryFn: async () => {
      const response = await apiClient.get("/api/app/proposal-template-block", {
        params: {
          maxResultCount: 1000,
        },
      });
      return response.data;
    },
  });
}

export function useProposalTemplateBlock(id: string) {
  return useQuery({
    queryKey: ["proposalTemplateBlock", id],
    queryFn: async () => {
      const response = await apiClient.get(`/api/app/proposal-template-block/${id}`);
      return response.data;
    },
    enabled: !!id,
  });
}

export function useCreateProposalTemplateBlock() {
  const queryClient = useQueryClient();
  return useMutation({
    mutationFn: async (data: any) => {
      const response = await apiClient.post("/api/app/proposal-template-block", data);
      return response.data;
    },
    onSuccess: () => {
      queryClient.invalidateQueries({ queryKey: ["proposalTemplateBlocks"] });
    },
  });
}

export function useUpdateProposalTemplateBlock() {
  const queryClient = useQueryClient();
  return useMutation({
    mutationFn: async ({ id, data }: { id: string; data: any }) => {
      const response = await apiClient.put(`/api/app/proposal-template-block/${id}`, data);
      return response.data;
    },
    onSuccess: (data) => {
      queryClient.invalidateQueries({ queryKey: ["proposalTemplateBlocks"] });
      queryClient.invalidateQueries({ queryKey: ["proposalTemplateBlock", data.id] });
    },
  });
}

export function useDeleteProposalTemplateBlock() {
  const queryClient = useQueryClient();
  return useMutation({
    mutationFn: async (id: string) => {
      await apiClient.delete(`/api/app/proposal-template-block/${id}`);
    },
    onSuccess: () => {
      queryClient.invalidateQueries({ queryKey: ["proposalTemplateBlocks"] });
    },
  });
}


